import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

function Sidebar() {
  const API_URL = import.meta.env.VITE_API_URL;
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    fetch(`${API_URL}Category`)
      .then(res => res.json())
      .then(data => setCategories(data))
      .catch(err => console.error(err));
  }, [API_URL]);

  return (
    <div style={{ width: '200px', backgroundColor: '#f0f0f0', height: '100vh', padding: '10px'}}>
      <ul style={{ listStyle: 'none', padding: 0 }}>
        {categories.map(cat => (
          <li key={cat.name} style={{ margin: '10px 0' }}>
            <Link to={`/${cat.id}`} style={{ color: 'black', textTransform: 'uppercase', textDecoration: 'none' }}>{cat.name}</Link>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default Sidebar;
