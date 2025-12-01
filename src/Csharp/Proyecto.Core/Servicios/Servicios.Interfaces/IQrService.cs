using Proyecto.Core.DTOs;

namespace Proyecto.Core.Servicios.Interfaces
{
    public interface IQrService
    {
        QrDTO GenerarQr(int idEntrada);
        byte[] GenerarQr(string qrContent);
        QrDTO ObtenerQrPorEntrada(int idEntrada);
        QrDTO ValidarQr(string qrContent);
    }
}
