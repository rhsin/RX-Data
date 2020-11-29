import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { UserTable } from './UserTable';
import {
  fetchUsers,
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

  return (
    <div>
      <div>Status: {status}</div>
      <div>Count: {users.length}</div>
      {status === 'loading' && <div>Loading...</div>}
      {message && <div>{message}</div>}
      <UserTable />
    </div>
  );
}

