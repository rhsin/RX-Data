import React from 'react';
import { RxPrices } from './features/rxPrices/RxPrices';
import { Users } from './features/users/Users';
import './App.css';

function App() {
  return (
    <div className='App'>
      <div>RxData</div>
      <Users />
      <RxPrices />
    </div>
  );
}

export default App;
