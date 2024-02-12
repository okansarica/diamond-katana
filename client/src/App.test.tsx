import React from 'react';
import { render, waitFor } from '@testing-library/react';
import App from './App';
import userEvent from '@testing-library/user-event';
import { act } from "react-dom/test-utils";
import { DiamondRow } from "./models/DiamondRow";
import postToDiamondKataApi from './services/DiamondKataService';

// Mock the DiamondKataService module
jest.mock('./services/DiamondKataService', () => ({
    __esModule: true,
    default: jest.fn(() => Promise.resolve<DiamondRow[]>([]))
}))

const mockPostToDiamondKataApi = postToDiamondKataApi as jest.MockedFunction<typeof postToDiamondKataApi>;


const setup = () => {
    const utils = render(<App/>);
    const input = utils.container.querySelector('.input-field') as HTMLElement;
    const button = utils.container.querySelector('.generate-button') as HTMLElement;
    return {
        input,
        button,
        ...utils,
    };
};

describe('<App />', () => {
    beforeEach(() => {
        mockPostToDiamondKataApi.mockClear();
    });

    it('renders without crashing', () => {
        //Arrange

        //Act
        const { input, button } = setup();

        //Assert
        expect(input).toBeInTheDocument();
        expect(button).toBeInTheDocument();
    });

    it('updates input field with valid input', async () => {
        //Arrange
        const { input } = setup();

        //Act
        await act(async () => {
            userEvent.type(input, 'A');
        })

        //Assert
        expect(input).toHaveValue('A');
    });

    it('updates input field with lower case valid input and makes it upper case', async () => {
        //Arrange
        const { input } = setup();

        //Act
        await act(async () => {
            userEvent.type(input, 's');
        })

        //Assert
        expect(input).toHaveValue('S');
    });

    it('clears input field and hides error message when user deletes from input', async () => {
        //Arrange
        const { input, container } = setup();

        //Act
        await act(async () => {
            //We need to set an initial value to be able to trigger clear event
            userEvent.type(input, 'r');
            userEvent.clear(input);
        })

        //Assert
        expect(input).toHaveValue('');
        expect(container.querySelector('.error-message')).toBeNull()
    });

    it('displays error for invalid input', async () => {
        //Arrange
        const { input, button, container } = setup();

        //Act
        await act(async () => {
            userEvent.type(input, '1');
            userEvent.click(button)
        });

        //Assert
        expect(container.querySelector('.error-message')).not.toBeNull()
    });

    it('displays loader in button when isLoading is true', async () => {
        //Arrange
        //We are setting a delay to wait the function to be return so we have enough time to check the loader
        const delayedPromise = new Promise<DiamondRow[]>(resolve => setTimeout(() => resolve([]), 3000));
        mockPostToDiamondKataApi.mockReturnValue(delayedPromise);

        const { input, button } = setup();

        //Act
        await act(async () => {
            userEvent.type(input, 'A');
            userEvent.click(button);
        })

        //Assert
        expect(button).toContainHTML('<div class="loader"></div>');
    });

    it('prevents second api call if the first api call is not resolved yet', async () => {
        //Arrange
        //We are setting a delay to wait the function to be return so we have enough time to click one more time before the api call is resolved
        const delayedPromise = new Promise<DiamondRow[]>(resolve => setTimeout(() => resolve([]), 3000));
        mockPostToDiamondKataApi.mockReturnValue(delayedPromise);

        const { input, button } = setup();

        //Act
        await act(async () => {
            userEvent.type(input, 'A');
            userEvent.click(button);
        })

        await act(async () => {
            //This is the second call that needs to be returned before the api call after the state is set
            userEvent.click(button);
        })

        //Assert
        expect(mockPostToDiamondKataApi).toHaveBeenCalledTimes(1);
    });

    it('removed loading when the api call is completed', async () => {
        //Arrange
        const promise = Promise.resolve([]);
        mockPostToDiamondKataApi.mockReturnValue(promise);

        const { input, button } = setup();

        //Act
        await act(async () => {
            userEvent.type(input, 'A');
            userEvent.click(button);
        })

        //Assert
        await waitFor(() => {
            expect(button).not.toContainHTML('<div class="loader"></div>');
            expect(button).toContainHTML('<span>Generate</span>');
        });
    });

    it('render the diamond with given response and input is cleared', async () => {
        //Arrange
        const diamondRows: DiamondRow[] = [{
            character: 'A',
            middleSpaceQuantity: 0,
            sideSpaceQuantity: 1,
            sortOrder: -1
        },
            {
                character: 'B',
                middleSpaceQuantity: 1,
                sideSpaceQuantity: 0,
                sortOrder: 0
            },
            {
                character: 'A',
                middleSpaceQuantity: 0,
                sideSpaceQuantity: 1,
                sortOrder: 1
            }]
        const promise = Promise.resolve(diamondRows);
        mockPostToDiamondKataApi.mockReturnValue(promise);

        const { input, button, container } = setup();

        //Act
        await act(async () => {
            userEvent.type(input, 'A');
            userEvent.click(button);
        })

        //Assert
        await waitFor(() => {
            const diamondContainer = container.querySelector('.diamond-container')
            expect(diamondContainer).not.toBeNull()
            expect(input).toHaveValue('');
        });
    });

    it('shows alert when the api response is not 200', async () => {
        //Arrange        
        const errorMessage = 'This is an error message';
        mockPostToDiamondKataApi.mockReturnValue(Promise.reject(new Error(errorMessage)));
        global.alert = jest.fn();

        const { input, button } = setup();

        //Act
        await act(async () => {
            userEvent.type(input, 'A');
            userEvent.click(button);
        })

        //Assert
        await waitFor(() => {
            expect(global.alert).toHaveBeenCalledWith(errorMessage);
            expect(button).not.toContainHTML('<div class="loader"></div>');
            expect(button).toContainHTML('<span>Generate</span>');
        })
    });
});

