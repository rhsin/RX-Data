import React from 'react';
import { Table, Button } from 'antd';
import { IdcardOutlined } from '@ant-design/icons';

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
      title: 'Account',
      key: 'account',
      render: (text, record) => (
        <Button 
          type='primary'
          icon={<IdcardOutlined />}
          onClick={() => console.log(record.id)}
        />
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

