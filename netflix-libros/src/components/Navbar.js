import { useState, useEffect } from "react";
import { FaBars, FaTimes } from "react-icons/fa";
import "./Navbar.css";

export default function Navbar() {
  const [open, setOpen] = useState(false);
  const [scrolled, setScrolled] = useState(false);

  const toggleMenu = () => setOpen(!open);

  // Detecta scroll para cambiar fondo
  useEffect(() => {
    const handleScroll = () => setScrolled(window.scrollY > 50);
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  return (
    <>
      <nav className={`navbar ${scrolled ? "scrolled" : ""}`}>
        <div className="logo">Netflix de Libros</div>

        <div className="hamburger-wrapper" onClick={toggleMenu}>
          <div className="hamburger-bg">
            {open ? <FaTimes size={28} /> : <FaBars size={28} />}
          </div>
        </div>
      </nav>

      <div className={`side-menu ${open ? "open" : ""}`}>
        <ul>
          <li><a href="#home" onClick={toggleMenu}>Inicio</a></li>
          <li><a href="#about" onClick={toggleMenu}>Sobre Nosotros</a></li>
          <li><a href="#services" onClick={toggleMenu}>Servicios</a></li>
          <li><a href="#contact" onClick={toggleMenu}>Contacto</a></li>
        </ul>
      </div>

      {open && <div className="overlay" onClick={toggleMenu}></div>}
    </>
  );
}