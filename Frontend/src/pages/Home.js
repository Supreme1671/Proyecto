import React from "react";
import BookRow from "../components/BookRow";

function Home() {
  const fiction = [
    {
      title: "Harry Potter",
      cover: "https://covers.openlibrary.org/b/id/10523353-L.jpg",
    },
    {
      title: "El Hobbit",
      cover: "https://covers.openlibrary.org/b/id/6979861-L.jpg",
    },
  ];

  const science = [
    {
      title: "Breve historia del tiempo",
      cover: "https://covers.openlibrary.org/b/id/11153218-L.jpg",
    },
    {
      title: "Cosmos",
      cover: "https://covers.openlibrary.org/b/id/240726-L.jpg",
    },
  ];

  return (
    <div style={{ background: "#000", minHeight: "100vh", paddingBottom: "2rem" }}>
      <BookRow category="Ficción" books={fiction} />
      <BookRow category="Ciencia" books={science} />
    </div>
  );
}

export default Home;