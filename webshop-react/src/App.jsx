import React, {useState} from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Category from './pages/Category';
import Cart from './pages/Cart';
import SidebarMenu from './components/SidebarMenu';
function App() {
  const [cart, setCart] = useState(null);
  return (
    <Router>
      <div style={{ display: 'flex' }}>
        <SidebarMenu />
        <div style={{ flex: 1, padding: '20px' }}>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/:category_id" element={<Category cart={cart} setCart={setCart} />} />
            <Route path="/cart/:cartId" element={<Cart />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;
