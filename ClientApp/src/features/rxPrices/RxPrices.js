import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Card, Input, Button, Spin, message as Message } from 'antd';
import { SearchOutlined, FileSearchOutlined, DollarOutlined } from '@ant-design/icons';
import { RxPriceTable } from './RxPriceTable';
import { addRxPrice, selectMessage } from '../users/usersSlice';
import {
  fetchRxPrices,
  findRxPrices,
  selectRxPrices,
  selectStatus,
  rxPricesUrl,
  webRxPricesUrl,
  webAltRxPricesUrl,
  findRxPricesUrl
} from './rxPricesSlice';

export function RxPrices() {
  const rxPrices = useSelector(selectRxPrices);
  const status = useSelector(selectStatus);
  const message = useSelector(selectMessage);
  const dispatch = useDispatch();
  const [name, setName] = useState('');

  useEffect(() => {
    if (status === 'idle') {
      dispatch(fetchRxPrices(rxPricesUrl));
    }
  }, [status, dispatch]);

  useEffect(() => {
    message && Message.success(message, 3);
  }, [message]);

  const saveRxPrice = (id) => {
    dispatch(addRxPrice(id, '1'));
  };

  return (
    <Card title='RxPrices'>
      <Input
        onChange={e => setName(e.target.value)}
        size = 'large'
        placeholder='Enter Medication'
      />
      <Button 
        className='btn-search'
        type='primary'
        icon={<FileSearchOutlined />}
        onClick={() => dispatch(findRxPrices(name))}
      >
        Find
      </Button>
      <Button 
        className='btn-search'
        type='primary'
        icon={<DollarOutlined />}
        onClick={() => dispatch(fetchRxPrices(findRxPricesUrl(name)))}
      >
        Price/Mg
      </Button>
      <Button
        className='btn-search'
        type='primary'
        icon={<SearchOutlined />}
        onClick={() => dispatch(fetchRxPrices(webRxPricesUrl(name)))}
      >
        Update
      </Button>
      <Button
        className='btn-search'
        type='primary'
        icon={<SearchOutlined />}
        onClick={() => dispatch(fetchRxPrices(webAltRxPricesUrl(name)))}
      >
        Update(Alt)
      </Button>
      {status === 'loading' && <div>Loading <Spin /></div>}
      <RxPriceTable 
        action='save'
        rxPrices={rxPrices} 
        handleRxPrice={id => saveRxPrice(id)}
      />
    </Card>
  );
}

