import '@testing-library/jest-dom';
import '@testing-library/jest-dom/extend-expect';

Object.defineProperty(window, 'matchMedia', {
  value: () => {
    return {
      matches: false,
      addListener: () => {},
      removeListener: () => {}
    };
  }
});

// jest.spyOn(global.console, 'log').mockImplementation(jest.fn());
