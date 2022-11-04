import React from 'react';
import { Route, Routes } from 'react-router-dom';
import { Layout } from './components/Layout';
import './custom.css';
import Home from './components/Home'

const App = () => {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Home/>}/>
      </Routes>
    </Layout>
  );
}

export default App;