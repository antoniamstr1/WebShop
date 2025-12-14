import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

const API_URL = import.meta.env.VITE_API_URL;

function Cart() {
  const { cartId } = useParams(); // assuming URL has /cart/:cartId
  const [cartItems, setCartItems] = useState([]);

  useEffect(() => {
    if (!cartId) return;

    fetch(`${API_URL}Cart/customer/${cartId}`)
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);
        return res.json();
      })
      .then((res) => setCartItems(res.productsInCart))
      .catch((err) => console.error("Failed to fetch products:", err));
  }, [cartId]);

  const handleAmountChange = (id, value) => {
    setCartItems((prev) =>
      prev.map((item) =>
        item.id === id ? { ...item, amount: Number(value) } : item
      )
    );
  };

  const totalPrice = cartItems.reduce(
    (sum, item) => sum + item.product.price * item.amount,
    0
  );

  return (
    <div style={{ display: "flex", justifyContent: "center", padding: "50px" }}>
      <div style={{ width: "600px" }}>
        <h2 style={{ textAlign: "center", marginBottom: "20px" }}>Cart</h2>
        {cartItems.map((item) => (
          <div key={item.id} style={{ display: "flex", justifyContent: "space-between", marginBottom: "10px" }}>
            <span style={{ flex: 2 }}>{item.product.name}</span>
            <span style={{ flex: 1 }}>${item.product.price}</span>
            <input
              type="number"
              min="1"
              value={item.amount}
              onChange={(e) => handleAmountChange(item.id, e.target.value)}
              style={{ flex: 1, textAlign: "center", width: "2rem" }}
            />
            <span style={{ flex: 1, textAlign: "right" }}>${item.product.price * item.amount}</span>
          </div>
        ))}
        <div style={{ display: "flex", justifyContent: "flex-end", fontWeight: "bold", marginTop: "20px" }}>
          Total: ${totalPrice}
        </div>
        <div style={{ display: "flex", justifyContent: "flex-end", marginTop: "2rem"}}><button style={{ }}>Continue</button></div>
        
      </div>
    </div>
  );
}

export default Cart;
