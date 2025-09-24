import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import "./LibroDetalle.css";

export default function LibroDetalle() {
  const { id } = useParams();
  const [libro, setLibro] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch(`https://localhost:5001/api/libros/${id}`) // URL de tu backend
      .then(res => res.json())
      .then(data => {
        setLibro(data);
        setLoading(false);
      })
      .catch(err => console.error(err));
  }, [id]);

  if (loading) return <p style={{ color: "white", textAlign: "center" }}>Cargando libro...</p>;
  if (!libro) return <p style={{ color: "white", textAlign: "center" }}>Libro no encontrado</p>;

  return (
    <div className="libro-detalle">
      <Link to="/" className="volver">← Volver</Link>
      <h1>{libro.titulo}</h1>
      <h3>{libro.autor}</h3>
      <img src={libro.urlPortada} alt={libro.titulo} />
      <p>{libro.descripcion}</p>
      <a href={libro.urlPdf} target="_blank" rel="noopener noreferrer" className="btn-pdf">
        Descargar PDF
      </a>
    </div>
  );
}
