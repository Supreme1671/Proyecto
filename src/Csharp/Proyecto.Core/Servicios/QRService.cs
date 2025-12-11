using System.Data.SqlTypes;
using Proyecto.Core.DTOs;
using Proyecto.Core.Entidades;
using Proyecto.Core.Repositorios;
using Proyecto.Core.Servicios.Interfaces;
using QRCoder;

namespace Proyecto.Core.Servicios
{
    public class QrService : IQrService
    {
        private readonly IEntradaRepository _entradaRepo;
        private readonly IQRRepository _qrRepo;

        public QrService(IEntradaRepository entradaRepo, IQRRepository qrRepo)
        {
            _entradaRepo = entradaRepo;
            _qrRepo = qrRepo;
        }

        public QrDTO GenerarQr(int idEntrada)
        {
            var entrada = _entradaRepo.GetById(idEntrada);
            if (entrada == null)
                throw new Exception("La entrada no existe");

            string contenido = $"http://localhost:5240/entrada/{entrada.IdEntrada}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrData = qrGenerator.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrData);
            string imagenBase64 = qrCode.GetGraphic(10);

            var qr = new QR
            {
                IdEntrada = idEntrada,
                Codigo = imagenBase64,
                FechaCreacion = DateTime.Now
            };

            _qrRepo.Add(qr);

            return new QrDTO
            {
                idQR = qr.idQR,
                IdEntrada = qr.IdEntrada,
                Codigo = qr.Codigo,
                FechaCreacion = qr.FechaCreacion
            };
        }
        public byte[] GenerarQr(string qrContent)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrData);
            return qrCode.GetGraphic(10);
        }

        public QrDTO ObtenerQrPorEntrada(int idEntrada)
        {
            var qr = _qrRepo.GetByEntrada(idEntrada);
            if (qr == null) return null;

            return new QrDTO
            {
                idQR = qr.idQR,
                IdEntrada = qr.IdEntrada,
                Codigo = qr.Codigo,
                FechaCreacion = qr.FechaCreacion
            };
        }

        public QrDTO Update(QR qr)
        {
            throw new NotImplementedException();
        }

        QrDTO IQrService.ValidarQr(string qrContent)
        {
            var resultado = new QrDTO();

            //Validar Contenido
            if (string.IsNullOrEmpty(qrContent))
            {
                resultado.Estado = "FirmaInvalida";
                return resultado;
            }

            try
            {
                //Parseo del QR
                var partes = qrContent.Split('|');
                if (partes.Length != 2)
                {
                    resultado.Estado = "FirmaInvalida";
                    return resultado;
                }

                var entradaPart = partes[0].Split(':');
                var usuarioPart = partes[1].Split(':');

                if (entradaPart.Length != 2 || usuarioPart.Length != 2)
                {
                    resultado.Estado = "FirmaInvalida";
                    return resultado;
                } 

                if (!int.TryParse(entradaPart[1], out int idEntrada))
                {
                    resultado.Estado = "FirmaInvalida";
                    return resultado;
                }

                //Verificar existencia de entrada

                var entrada = _entradaRepo.GetById(idEntrada);
                if (entrada == null)
                {
                    resultado.Estado = "No existe";
                    return resultado;
                }

                //Buscar QR asociado 
                var qr = _qrRepo.GetByEntrada(idEntrada);
                if (qr == null)
                {
                    resultado.Estado = "No existe";
                    return resultado;
                }

                // Validar firma del QR (contenido coincide con el QR generado)
                string contenidoEsperado = $"Entrada:{entrada.IdEntrada}|Usuario:{entrada.IdEntrada}";
                if (qrContent != contenidoEsperado)
                {
                    resultado.Estado = "FirmaInvalida";
                    return resultado;
                }

                //Verificar si ya fue usada
                if (qr.FechaUso != null)
                {
                    resultado.Estado = "Ya Usada";
                    return resultado;   
                }

                //Verificar si esta expirada
                if((DateTime.Now - qr.FechaCreacion).TotalHours > 24)
                {
                    resultado.Estado = "Expirada";
                    return resultado;
                }

                // Todo ok marca como usada
                qr.FechaUso = DateTime.Now;
                _qrRepo.Update(qr);

                resultado.Estado = "Ok";
                resultado.idQR = qr.idQR;
                resultado.IdEntrada = qr.IdEntrada;
                resultado.Codigo = qr.Codigo;
                resultado.FechaCreacion = qr.FechaCreacion;

                return resultado;
            }
            catch
            {
                resultado.Estado = "FirmaInvalida";
                return resultado;
            }

        }
    }
}
