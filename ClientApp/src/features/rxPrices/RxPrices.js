import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Input, Button, Spin } from 'antd';
import { RxPriceTable } from './RxPriceTable';
import { addRxPrice } from '../users/usersSlice';
import {
  fetchRxPrices,
  findRxPrices,
  selectRxPrices,
  selectStatus,
  rxPricesUrl,
  webRxPricesUrl,
  webAltRxPricesUrl,
  findRxPricesUrl
} from './rxPricesSlice';

export function RxPrices() {
  const rxPrices = useSelector(selectRxPrices);
  const status = useSelector(selectStatus);
  const dispatch = useDispatch();
  const [name, setName] = useState('');

  useEffect(() => {
    if (status === 'idle') {
      dispatch(fetchRxPrices(rxPricesUrl))
    }
  }, [status, dispatch]);

  const saveRxPrice = (id) => dispatch(addRxPrice(id, '1'));

  return (
    <div>
      <Input
        onChange={e => setName(e.target.value)}
        size = 'large'
        placeholder='Enter Medication'
      />
      <Button 
        type='primary'
        shape= 'round'
        onClick={() => dispatch(findRxPrices(name))}
      >
        Find
      </Button>
      <Button 
        type='primary'
        shape= 'round'
        onClick={() => dispatch(fetchRxPrices(findRxPricesUrl(name)))}
      >
        $/Mg
      </Button>
      <Button
        type='primary'
        shape= 'round'
        onClick={() => dispatch(fetchRxPrices(webRxPricesUrl(name)))}
      >
        Update
      </Button>
      <Button
        type='primary'
        shape= 'round'
        onClick={() => dispatch(fetchRxPrices(webAltRxPricesUrl(name)))}
      >
        Update(Alt)
      </Button>
      {status === 'loading' && <div>Loading <Spin /></div>}
      <RxPriceTable 
        rxPrices={rxPrices} 
        handleRxPrice={id => saveRxPrice(id)}
      />
    </div>
  );
}

