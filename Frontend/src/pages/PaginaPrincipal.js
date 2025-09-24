import React, { useEffect, useState } from "react";
import BookCard from "../components/BookCard";
import "../Styles/PaginaPrincipal.css";

export default function PaginaPrincipal() {
  const [libros, setLibros] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch("http://localhost:5072/api/Libros") // Ajustá el puerto según tu backend
      .then((res) => res.json())
      .then((data) => {
        setLibros(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Error al obtener libros:", err);
        setLoading(false);
      });
  }, []);

  if (loading) return <div className="cargando">Cargando libros...</div>;

  return (
    <main>
      <div className="grid">
        {libros.map((libro) => (
          <BookCard
            key={libro.id}
            title={libro.titulo}
            author={libro.autor}
            cover={libro.urlPortada}
          />
        ))}
      </div>
    </main>
  );
}
