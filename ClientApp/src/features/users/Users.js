import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  fetchUsers,
  removeRxPrice,
  selectUsers,
  selectStatus,
  selectMessage,
  selectError
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

  return (
    <div>
      <div>Users</div>
      <div>Status: {status}</div>
      <div>Count: {users.length}</div>
      {status === 'loading' && <div>Loading...</div>}
      {message && <div>{message}</div>}
      {users.map(user => 
        <div key={user.id}>
          <div>{user.id}</div>  
          <div>{user.name}</div>  
          <div>{user.email}</div>  
          <div>RxPrices: {user.rxPriceUsers.length}</div> 
          <button 
            onClick={() => dispatch(removeRxPrice('2', user.id))}
          >
            -
          </button> 
        </div>
      )}
    </div>
  );
}

