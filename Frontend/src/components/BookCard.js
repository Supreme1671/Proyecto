import React from "react";

function BookCard({ title, cover }) {
  return (
    <div style={{ width: "150px", margin: "10px" }}>
      <img
        src={cover}
        alt={title}
        style={{ width: "100%", borderRadius: "10px" }}
      />
      <p style={{ color: "white", textAlign: "center" }}>{title}</p>
    </div>
  );
}

export default BookCard;