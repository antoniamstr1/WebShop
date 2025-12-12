import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Category from './pages/Category';
import SidebarMenu from './components/SidebarMenu';
function App() {
  return (
    <Router>
      <div style={{ display: 'flex' }}>
        <SidebarMenu />
        <div style={{ flex: 1, padding: '20px' }}>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/:category_id" element={<Category />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;
