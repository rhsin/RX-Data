import React from 'react';
import { Table, Button } from 'antd';

export function RxPriceTable({ rxPrices, handleRxPrice }) {
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
      title: 'Quantity',
      dataIndex: 'quantity',
      key: 'quantity',
    },
    {
      title: 'Dose(mg)',
      dataIndex: 'dose',
      key: 'dose',
    },
    {
      title: 'Price',
      dataIndex: 'price',
      key: 'price',
    },
    {
      title: 'Location',
      dataIndex: 'location',
      key: 'location',
    },
    {
      title: 'Vendor',
      dataIndex: ['vendor', 'name'],
      key: 'vendor',
    },
    {
      title: 'Action',
      key: 'action',
      render: (text, record) => (
        <Button 
          type='primary'
          shape= 'circle'
          onClick={() => handleRxPrice(record.id)}
        >
          A
        </Button> 
      )
    }
  ];

  return (
    <Table 
      columns={columns}
      dataSource={rxPrices}
    />
  );
}

