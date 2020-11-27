import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  fetchRxPrices,
  findRxPrices,
  selectRxPrices,
  selectStatus,
  selectError
} from './rxPricesSlice';

export function RxPrices() {
  const rxPrices = useSelector(selectRxPrices);
  const status = useSelector(selectStatus);
  const dispatch = useDispatch();
  const [name, setName] = useState('');

  useEffect(() => {
    if (status === 'idle') {
      dispatch(fetchRxPrices())
    }
  }, [status, dispatch]);

  return (
    <div>
      <div>RxPrices</div>
      <div>Status: {status}</div>
      <div>Count: {rxPrices.length}</div>
      <input onChange={e => setName(e.target.value)} />
      <button onClick={() => dispatch(findRxPrices(name))}>
        Find
      </button>
      {status === 'loading' && <div>Loading...</div>}
      {rxPrices.map(rxPrice => 
        <div key={rxPrice.id}>
          <div>{rxPrice.id}</div>  
          <div>{rxPrice.name}</div>  
          <div>{rxPrice.price}</div>  
        </div>
      )}
    </div>
  );
}

