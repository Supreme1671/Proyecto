import React from "react";
import BookCard from "./BookCard";

function BookRow({ category, books }) {
  return (
    <div style={{ margin: "20px" }}>
      <h2 style={{ color: "white" }}>{category}</h2>
      <div style={{ display: "flex", overflowX: "scroll" }}>
        {books.map((book, index) => (
          <BookCard key={index} title={book.title} cover={book.cover} />
        ))}
      </div>
    </div>
  );
}

export default BookRow;