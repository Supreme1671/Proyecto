import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import PaginaPrincipal from "./pages/PaginaPrincipal";
import Categoria from "./components/Categoria";

function App() {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/" element={<PaginaPrincipal />} />
        <Route path="/categoria" element={<Categoria />} />
      </Routes>
    </Router>
  );
}

export default App;
