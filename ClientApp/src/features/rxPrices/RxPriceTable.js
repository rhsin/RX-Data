import React from 'react';
import { Table, Button } from 'antd';
import { DownloadOutlined, DeleteOutlined } from '@ant-design/icons';

export function RxPriceTable(props) {
  const { action, rxPrices, handleRxPrice } = props;

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
      title: action === 'save' ? 'Add' : 'Remove',
      key: 'action',
      render: (text, record) => (
        <Button 
          className='btn-action'
          type='primary'
          shape='circle'
          icon={action === 'save' ? <DownloadOutlined /> : <DeleteOutlined />}
          onClick={() => handleRxPrice(record.id)}
        />
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

