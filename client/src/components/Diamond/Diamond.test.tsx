import React from 'react';
import { render } from '@testing-library/react';
import Diamond from './Diamond';
import { DiamondRow } from "../../models/DiamondRow";


const mockDiamondRows: DiamondRow[] = [{
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
const setup = () => {
    const utils = render(<Diamond diamondRows={mockDiamondRows}/>);
    const diamondContainer = utils.container.querySelector('.diamond-container')
    return {
        diamondContainer,
        ...utils,
    };
};

describe('Diamond Component', () => {
    it('renders without crashing', () => {
        //Arrange

        //Act
        const { diamondContainer } = setup();

        //Assert
        expect(diamondContainer).toBeInTheDocument();
    });

    it('renders the diamond shape correctly based on props', () => {
        //Arrange
        
        //Act
        const { diamondContainer } = setup();

        //Assert
        expect(diamondContainer).toContainHTML(
            '<div class="diamond-row">-A-</div><div class="diamond-row">B-B</div><div class="diamond-row">-A-</div>')
    });
});
