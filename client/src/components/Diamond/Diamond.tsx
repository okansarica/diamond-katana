import React from 'react';
import './Diamond.css';
import { DiamondRow } from "../../models/DiamondRow";

interface DiamondProps {
    diamondRows: DiamondRow[]
}

const Diamond: React.FC<DiamondProps> = ({ diamondRows }) => {

    const renderDiamondRow = (diamondRow: DiamondRow) => {
        //The underscore character did seem merged so used - as separator
        const separator = '-'

        let content = '';

        //Add the separators for left side of the character 
        content += separator.repeat(diamondRow.sideSpaceQuantity);

        //Add the character
        content += diamondRow.character;

        //If this is not the first or latest row
        if (diamondRow.middleSpaceQuantity) {

            //Add the middle spaces between the characters
            content += separator.repeat(diamondRow.middleSpaceQuantity);

            //Add the second character
            content += diamondRow.character;
        }

        //Add the separators for right side of the character
        content += separator.repeat(diamondRow.sideSpaceQuantity);

        return content;
    }

    return (
        <div className="diamond-container">
            {diamondRows.map(
                (row: DiamondRow) => <div className="diamond-row" key={row.sortOrder}>{renderDiamondRow(
                    row)}</div>)}
        </div>
    )
};

export default Diamond;
