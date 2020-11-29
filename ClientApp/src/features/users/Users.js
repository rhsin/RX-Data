import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Spin } from 'antd';
import { UserTable } from './UserTable';
import { RxPriceTable } from '../rxPrices/RxPriceTable';
import {
  fetchUsers,
  removeRxPrice,
  selectUsers,
  selectStatus,
  selectMessage
} from './usersSlice';

export function Users() {
  const users = useSelector(selectUsers);
  const status = useSelector(selectStatus);
  const message = useSelector(selectMessage);
  const dispatch = useDispatch();

  useEffect(() => {
    if (status === 'idle') {
      dispatch(fetchUsers())
    }
  }, [users, status, dispatch]);

  const rxPrices = users[0] && users[0].rxPrices.map(item => item.rxPrice);

  const deleteRxPrice = (id) => dispatch(removeRxPrice(id, '1'));

  return (
    <div>
      {status === 'loading' && <div>Loading <Spin /></div>}
      {message && <div>{message}</div>}
      <UserTable users={users} />
      <RxPriceTable 
        rxPrices={rxPrices} 
        handleRxPrice={id => deleteRxPrice(id)}
      />
    </div>
  );
}

