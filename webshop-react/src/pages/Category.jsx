import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Link } from "react-router-dom";

function Category() {
  const { category_id } = useParams();
  const [products, setProducts] = useState([]);
  const [cart, setCart] = useState(null);

  const API_URL = import.meta.env.VITE_API_URL;

  useEffect(() => {
    if (!category_id) return;

    const getRandomColor = () => {
      const letters = "0123456789ABCDEF";
      let color = "#";
      for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
      }
      return color;
    };

    fetch(`${API_URL}Product/${category_id}`)
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);
        return res.json();
      })
      .then((data) => {
        const productsWithColors = data.map((p) => ({
          ...p,
          color: getRandomColor(),
        }));
        setProducts(productsWithColors);
      })
      .catch((err) => console.error("Failed to fetch products:", err));
  }, [category_id]);

  const handleAddToCart = async (productId) => {
    try {
      const response = await fetch(`${API_URL}Cart/add`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          ProductId: productId,
          CartId: cart,
        }),
      });

      if (!response.ok) {
        throw new Error("Failed to add product to cart");
      }

      if (cart === null) {
        const cartID = await response.json();
        setCart(cartID);
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div>
      <div
        style={{
          display: "flex",
          flexWrap: "wrap",
          justifyContent: "center",
          gap: "30px",
          position: "relative",
        }}
      >
        {products.map((product) => (
          <div
            key={product.id}
            style={{
              display: "flex",
              flexDirection: "column",
              gap: "5px",
            }}
          >
            <div
              style={{
                width: "200px",
                height: "200px",
                backgroundColor: product.color,
                borderRadius: "4px",
              }}
            />
            <div
              style={{
                display: "flex",
                flexDirection: "row",
                justifyContent: "space-around",
                alignItems: "center",
              }}
            >
              <div
                style={{
                  display: "flex",
                  flexDirection: "column",
                }}
              >
                <strong style={{ fontSize: "12px" }}>{product.name}</strong>
                <span style={{ fontSize: "10px" }}>{product.description}</span>
                <span style={{ fontSize: "12px", fontWeight: "bold" }}>
                  ${product.price}
                </span>
              </div>
              <button onClick={() => handleAddToCart(product.id)}>
                <img
                  src="/images/add.png"
                  alt="cart"
                  style={{
                    width: "1.5rem",
                    height: "1.5rem",
                    cursor: "pointer",
                  }}
                />
              </button>
            </div>
          </div>
        ))}
      </div>
      <div>
        <Link to={`/cart/${cart}`}>
          <img
            src="/images/cart.png"
            alt="cart"
            style={{
              width: "2rem",
              height: "2rem",
              position: "fixed",
              top: "2rem",
              right: "2rem",
              cursor: "pointer",
            }}
          />
        </Link>
      </div>
    </div>
  );
}

export default Category;
