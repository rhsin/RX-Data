import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  fetchRxPrices,
  selectRxPrices,
  selectStatus,
  selectError
} from './rxPricesSlice';

export function RxPrices() {
  const rxPrices = useSelector(selectRxPrices);
  const status = useSelector(selectStatus);
  const dispatch = useDispatch();

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

