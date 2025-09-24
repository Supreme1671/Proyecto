import { useState } from "react";
import { FaBars, FaTimes } from "react-icons/fa";
import "./Navbar.css";

export default function Navbar() {
  const [open, setOpen] = useState(false);

  const toggleMenu = () => {
    setOpen(!open);
  };

  return (
    <nav className="navbar">
      <h1>Netflix de Libros</h1>

      <button className="menu-toggle" onClick={toggleMenu}>
        {open ? <FaTimes /> : <FaBars />}
      </button>

      <div className={`menu ${open ? "open" : ""}`}>
        <a href="/">Inicio</a>
        <a href="/sobre-nosotros">Sobre Nosotros</a>
        <a href="/categoria">Categorías</a>
        <a href="/contacto">Contacto</a>
      </div>
    </nav>
  );
}
