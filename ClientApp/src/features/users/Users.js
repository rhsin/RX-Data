import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  fetchUsers,
  selectUsers,
  selectStatus,
  selectError
} from './usersSlice';

export function Users() {
  const users = useSelector(selectUsers);
  const status = useSelector(selectStatus);
  const dispatch = useDispatch();

  useEffect(() => {
    if (status === 'idle') {
      dispatch(fetchUsers())
    }
  }, [status, dispatch]);

  return (
    <div>
      <div>Users</div>
      <div>Status: {status}</div>
      <div>Count: {users.length}</div>
      {status === 'loading' && <div>Loading...</div>}
      {users.map(user => 
        <div key={user.id}>
          <div>{user.id}</div>  
          <div>{user.name}</div>  
          <div>{user.email}</div>  
        </div>
      )}
    </div>
  );
}

