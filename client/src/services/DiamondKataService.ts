import { DiamondRow } from "../models/DiamondRow";


//TODO API calls can be encapsulated with a generic function to improve consistency and remove code duplication when there are multiple calls
const postToDiamondKataApi = async (character: string): Promise<DiamondRow[]> => {
    const response = await fetch(`${process.env.REACT_APP_API_URL}/diamondkata`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ character }),
    });

    if (!response.ok) {
        if (response.status === 400) {
            const content = await response.json()
            throw new Error(content.message)
        }
        throw new Error(`API call failed with status: ${response.status}`);
    }

    return response.json();
};

export default postToDiamondKataApi;
