import React from "react";
import "../Styles/BookCard.css";

export default function BookCard({ title, author, cover }) {
  return (
    <div style={{ width: "150px", margin: "10px" }}>
      <img
        src={cover}
        alt={title}
        style={{ width: "100%", borderRadius: "10px" }}
      />
      <p style={{ color: "white", textAlign: "center", fontWeight: "bold" }}>{title}</p>
      <p style={{ color: "#aaa", textAlign: "center", fontSize: "0.85rem" }}>{author}</p>
    </div>
  );
}
