import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Table, Button } from 'antd';
import { removeRxPrice, selectUsers } from './usersSlice';

export function UserTable() {
  const users = useSelector(selectUsers);
  const dispatch = useDispatch();

  const columns = [
    {
      title: 'Id',
      dataIndex: 'id',
      key: 'id',
    },
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Email',
      dataIndex: 'email',
      key: 'email',
    },
    {
      title: 'Action',
      key: 'action',
      render: (text, record) => (
        <Button 
          type='primary'
          shape= 'round'
          onClick={() => dispatch(removeRxPrice('2', record.id))}
        >
          -
        </Button> 
      )
    }
  ];

  return (
    <Table columns={columns} dataSource={users} />
  );
}

