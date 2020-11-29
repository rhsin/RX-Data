import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Table, Button } from 'antd';
import { selectRxPrices } from './rxPricesSlice';
import { addRxPrice } from '../users/usersSlice';

export function RxPriceTable() {
  const rxPrices = useSelector(selectRxPrices);
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
      title: 'Price',
      dataIndex: 'price',
      key: 'price',
    },
    {
      title: 'Action',
      key: 'action',
      render: (text, record) => (
        <Button 
          type='primary'
          shape= 'round'
          onClick={() => dispatch(addRxPrice(record.id, '1'))}
        >
          +
        </Button> 
      )
    }
  ];

  return (
    <Table columns={columns} dataSource={rxPrices} />
  );
}

