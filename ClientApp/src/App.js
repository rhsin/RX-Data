import React from 'react';
import { Card, PageHeader } from 'antd';
import { RxPrices } from './features/rxPrices/RxPrices';
import { Users } from './features/users/Users';
import './App.css';

function App() {
  return (
    <Card className='App'>
      <PageHeader 
        onBack={() => null}
        title='RxData'
        subTitle='Dashboard'
      />
      <Users />
      <RxPrices />
    </Card>
  );
}

export default App;
