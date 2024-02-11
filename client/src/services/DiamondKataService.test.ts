import postToDiamondKataApi from './DiamondKataService';
import { DiamondRow } from "../models/DiamondRow";

describe('postToDiamondKataApi', () => {

    beforeEach(() => {
        jest.resetAllMocks();
    });

    it('should send the correct character to the API', async () => {
        // Arrange
        const mockResponse = [{
            character: 'A',
            sideSpaceQuantity: 0,
            middleSpaceQuantity: 0,
            sortOrder: 0
        } as DiamondRow];
        global.fetch = jest.fn().mockResolvedValue({
            ok: true,
            json: () => Promise.resolve(mockResponse),
        });

        // Act
        await postToDiamondKataApi('A');

        // Assert
        expect(fetch).toHaveBeenCalledWith(expect.any(String), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ character: 'A' }),
        });
    });

    it('should throw an error with message when API call fails with status 400', async () => {
        // Arrange
        const mockErrorResponse = { message: 'Input data is not valid.' };
        global.fetch = jest.fn().mockResolvedValue({
            ok: false,
            status: 400,
            json: () => Promise.resolve(mockErrorResponse),
        });

        // Act & Assert
        await expect(postToDiamondKataApi('A')).rejects.toThrow('Input data is not valid.');
    });

    it('should throw an error with user friendly message when API call fails with status 500', async () => {
        // Arrange
        global.fetch = jest.fn().mockResolvedValue({
            ok: false,
            status: 500,
        });

        // Act & Assert
        await expect(postToDiamondKataApi('A')).rejects.toThrow('API call failed with status: 500');
    });

    it('should return the expected response data on a successful API call', async () => {
        // Arrange
        const mockSuccessResponse = [
            {
                character: 'A',
                sideSpaceQuantity: 0,
                middleSpaceQuantity: 0,
                sortOrder: 0
            }
        ];

        global.fetch = jest.fn().mockResolvedValue({
            ok: true,
            json: () => Promise.resolve(mockSuccessResponse),
        });

        // Act
        const response = await postToDiamondKataApi('A');

        // Assert
        expect(response).toEqual(mockSuccessResponse);
    });
});
