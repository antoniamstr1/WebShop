import React, { useState } from "react";

function Cart() {
  // Example products in the cart
  const [cartItems, setCartItems] = useState([
    { id: 1, name: "Laptop", price: 1200, amount: 1 },
    { id: 2, name: "Mouse", price: 30, amount: 2 },
    { id: 3, name: "Keyboard", price: 50, amount: 1 },
  ]);

  // Calculate total price
  const totalPrice = cartItems.reduce(
    (sum, item) => sum + item.price * item.amount,
    0
  );

  // Handle amount change
  const handleAmountChange = (id, value) => {
    setCartItems((prev) =>
      prev.map((item) =>
        item.id === id ? { ...item, amount: Number(value) } : item
      )
    );
  };

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        padding: "50px",
        minHeight: "100vh",
        boxSizing: "border-box",
      }}
    >
      <div style={{ width: "600px" }}>
        <h2 style={{ textAlign: "center", marginBottom: "20px" }}>Cart</h2>

        {/* Table header */}
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            fontWeight: "bold",
            borderBottom: "1px solid #ccc",
            paddingBottom: "10px",
            marginBottom: "10px",
          }}
        >
          <span style={{ flex: 2 }}>Product</span>
          <span style={{ flex: 1, textAlign: "right" }}>Price</span>
          <span style={{ flex: 1, textAlign: "center" }}>Amount</span>
          <span style={{ flex: 1, textAlign: "right" }}>Total</span>
        </div>

        {/* Cart items */}
        {cartItems.map((item) => (
          <div
            key={item.id}
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              marginBottom: "10px",
            }}
          >
            <span style={{ flex: 2 }}>{item.name}</span>
            <span style={{ flex: 1, textAlign: "right" }}>${item.price}</span>
            <input
              type="number"
              min="1"
              value={item.amount}
              onChange={(e) => handleAmountChange(item.id, e.target.value)}
              style={{
                flex: 1,
                textAlign: "center",
                padding: "2px 5px",
                width: "50px",
                border: "none"
              }}
            />
            <span style={{ flex: 1, textAlign: "right" }}>
              ${item.price * item.amount}
            </span>
          </div>
        ))}

        <div
          style={{
            display: "flex",
            justifyContent: "flex-end",
            fontWeight: "bold",
            marginTop: "20px",
          }}
        >
          Total: ${totalPrice}
        </div>

        <div style={{ display: "flex", justifyContent: "flex-end", marginTop: "20px" }}>
          <button
            style={{
              padding: "10px 20px",
              backgroundColor: "#a6b6c6ff",
              color: "white",
              border: "none",
              borderRadius: "4px",
              cursor: "pointer",
            }}
          >
            Next
          </button>
        </div>
      </div>
    </div>
  );
}

export default Cart;
