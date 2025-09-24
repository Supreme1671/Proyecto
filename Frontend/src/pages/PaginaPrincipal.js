import "./PaginaPrincipal.css";
import { useEffect, useState } from "react";
import BookCard from "../components/BookCard"; // Asegurate que la ruta sea correcta

export default function PaginaPrincipal() {
  const [libros, setLibros] = useState([]);

  useEffect(() => {
    fetch("https://localhost:5072/api/libros") // Cambiar puerto si corresponde
      .then((res) => res.json())
      .then((data) => setLibros(data))
      .catch((err) => console.error("Error al obtener libros:", err));
  }, []);

  return (
    <main>
      <h1 style={{ color: "white", textAlign: "center" }}>Netflix de Libros</h1>
      
      {libros.length === 0 ? (
        <p style={{ color: "white", textAlign: "center" }}>Cargando libros...</p>
      ) : (
        <div className="grid">
          {libros.map((libro) => (
            <BookCard
              key={libro.id}
              title={libro.titulo}
              cover={libro.urlPortada}
            />
          ))}
        </div>
      )}
    </main>
  );
}
