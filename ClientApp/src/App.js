import React from 'react';
import { RxPrices } from './features/rxPrices/RxPrices';
import { Users } from './features/users/Users';
import './App.css';

function App() {
  return (
    <div className='App'>
      <header className='App-header'>
        <div>RxData</div>
        <Users />
        <RxPrices />
      </header>
    </div>
  );
}

export default App;
