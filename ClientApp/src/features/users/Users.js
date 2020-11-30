import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Card, Collapse, Spin, message as Message } from 'antd';
import { UserTable } from './UserTable';
import { RxPriceTable } from '../rxPrices/RxPriceTable';
import {
  fetchUsers,
  removeRxPrice,
  selectUsers,
  selectStatus,
  selectMessage
} from './usersSlice';

const { Panel } = Collapse;

export function Users() {
  const users = useSelector(selectUsers);
  const status = useSelector(selectStatus);
  const message = useSelector(selectMessage);
  const dispatch = useDispatch();

  useEffect(() => {
    if (status === 'idle') {
      dispatch(fetchUsers());
    }
  }, [users, status, dispatch]);

  useEffect(() => {
    message && Message.success(message, 3);
  }, [message]);

  const rxPrices = users[0] && users[0].rxPrices.map(item => item.rxPrice);

  const deleteRxPrice = (id) => {
    dispatch(removeRxPrice(id, '1'));
  };

  return (
    <Card title='Users'>
      {status === 'loading' && <div>Loading <Spin /></div>}
      <UserTable users={users} />
      <Collapse defaultActiveKey={['1']}>
        <Panel header='User-RxPrices' key='1'>
          <RxPriceTable 
            action='delete'
            rxPrices={rxPrices} 
            handleRxPrice={id => deleteRxPrice(id)}
          />
        </Panel>
      </Collapse>
    </Card>
  );
}

