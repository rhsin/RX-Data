import React from 'react';
import { Table, Button } from 'antd';

export function UserTable({ users }) {
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
          shape= 'circle'
          onClick={() => console.log(record.id)}
        >
          A
        </Button> 
      )
    }
  ];

  return (
    <Table 
      columns={columns} 
      dataSource={users} 
    />
  );
}

