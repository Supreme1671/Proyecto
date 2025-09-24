import React, { useEffect, useState } from "react";
import "../Styles/Categoria.css";

export default function Categoria() {
  const [categorias, setCategorias] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch("http://localhost:5072/api/categorias") // Ajustá el puerto si tu backend usa otro
      .then(res => res.json())
      .then(data => {
        setCategorias(data);
        setLoading(false);
      })
      .catch(err => {
        console.error("Error al cargar categorías:", err);
        setLoading(false);
      });
  }, []);

  if (loading) return <p className="loading">Cargando categorías...</p>;

  return (
    <main className="categoria-page">
      <h1>Categorías de Libros</h1>
      <div className="categoria-grid">
        {categorias.map(cat => (
          <div key={cat.id} className="categoria-card">
            {cat.nombre}
          </div>
        ))}
      </div>
    </main>
  );
}
