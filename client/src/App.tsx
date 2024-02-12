import React, { useState, ChangeEvent, useRef } from 'react';
import './App.css';
import { DiamondRow } from "./models/DiamondRow";
import postToDiamondKataApi from "./services/DiamondKataService";
import Diamond from "./components/Diamond/Diamond";

const App: React.FC = () => {
    const [input, setInput] = useState<string>('');
    const [error, setError] = useState<string>('');
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [diamondRows, setDiamondRows] = useState<DiamondRow[]>([]);

    const inputRef = useRef<HTMLInputElement>(null);

    const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
        const value = event.target.value.toUpperCase();
        if (!value) {
            setError('');
            setInput(value);
            return
        }
        const validationResult = isInputValid(value);
        if (!validationResult.isValid) {
            setError(validationResult.message)
            return
        }
        setError('');
        setInput(value);
    };

    const handleButtonClick = async () => {
        if (isLoading) {
            return
        }
        const validationResult = isInputValid(input);
        if (!validationResult.isValid) {
            inputRef.current?.focus()
            setError(validationResult.message)
            return
        }
        setError('');
        setIsLoading(true);

        let diamondRows: DiamondRow[] = [];
        try {
            diamondRows = await postToDiamondKataApi(input);
        }
        catch (error: any) {
            //TODO A more common way can be applied when there are more calls in the project
            alert(error.message)
            return
        }
        finally {
            setIsLoading(false);
        }

        setDiamondRows(diamondRows)
        setInput('')
    };

    const isInputValid = (inputToValidate: string): { isValid: boolean; message: string } => {
        //Check the input to be a single capital case character from A to Z (English alphabet)
        const isValid = /^[A-Z]$/.test(inputToValidate);
        let message = '';

        if (!isValid) {
            message = 'Input must be a single capital letter from A to Z (English alphabet).';
        }
        return { isValid, message };
    };

    return (
        <div className="app">
            <header className="app-header">
                <h1>The Diamond Kata</h1>
            </header>
            <main className="app-content">
                <div className="row">
                    <input
                        type="text"
                        className={`input-field ${error ? 'input-error' : ''}`}
                        value={input}
                        onChange={handleInputChange}
                        placeholder="Enter the character for the diamond"
                        maxLength={1}
                        ref={inputRef}
                    />
                    <button
                        className="generate-button"
                        onClick={handleButtonClick}>
                        {isLoading ? (
                            <div className="loader"></div>
                        ) : (
                            <span>Generate</span>
                        )}
                    </button>
                </div>
                {error && <div className="row">
                    <span className="error-message">{error}</span>
                </div>}
                {diamondRows?.length?<Diamond diamondRows={diamondRows}/>:null}                
            </main>
        </div>
    );
}

export default App;
