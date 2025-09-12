import "./PaginaPrincipal.css";

export default function PaginaPrincipal() {
  return (
    <>
      <header>
        <h1>Netflix de Libros</h1>
        <nav>
          <a href="#" className="active">Inicio</a>
          <a href="/catalogo">Catálogo</a>
          <a href="#">Mi Lista</a>
          <a href="#">Buscar</a>
        </nav>
      </header>

      <main>
        <div className="grid">
          <a
            href="/Cuentos/20MilLenguasDeViajeSubmarino/20MilLenguasdeViajeSubmarino.html"
            className="book"
            title="Ver detalles de 20 Mil Leguas de Viaje Submarino"
          >
            <img
              src="/Cuentos/20MilLenguasDeViajeSubmarino/COVER-Ventimila-Leghe-Sotto-i-Mari-SPA-CC2022-FRONTAL.jpg"
              alt="20 Mil Leguas de Viaje Submarino"
            />
            <div className="book-info">
              <div className="book-title">20 Mil Leguas de Viaje Submarino</div>
              <div className="book-author">Julio Verne</div>
            </div>
          </a>

          <a
            href="/Cuentos/El Principito/El principito.html"
            className="book"
            title="Ver detalles de El principito"
          >
            <img
              src="/Cuentos/El Principito/el principito.jpeg"
              alt="El principito"
            />
            <div className="book-info">
              <div className="book-title">El principito</div>
              <div className="book-author">Antoine de Saint-Exupéry</div>
            </div>
          </a>

          {/* acá podés agregar más libros */}
        </div>
      </main>
    </>
  );
}