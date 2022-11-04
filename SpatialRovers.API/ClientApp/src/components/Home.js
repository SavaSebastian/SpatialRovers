import React, { useState } from "react";
import Papa from "papaparse";

const allowedExtensions = ["csv"];

const Home = () => {
    const [file, setFile] = useState("");
    const [board, setBoard] = useState(Array(6).fill(0).map(() => new Array(6).fill("")));

    const handleFileChange = (e) => {
        if (e.target.files.length) {
            const inputFile = e.target.files[0];

            const fileExtension = inputFile?.type.split("/")[1];

            if (!allowedExtensions.includes(fileExtension)) {
                return;
            }

            setFile(inputFile);
        }
    };

    const handleParse = async () => {
        if (!file) return

        const reader = new FileReader();

        reader.onload = async ({ target }) => {
            Papa.parse(target.result, {
                delimiter: ",",
                newline: "\r\n",
                transformHeader: function (h) {
                    return h.trim();
                },
                complete: async (result) => {
                    console.log(result.data);
                    const response = await fetch('/api/plateau/initializerovers', {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify(result.data),
                        credentials: "include"
                    });
                    const content = await response.json();
                    console.log(content);
                }
            });
        };

        reader.readAsText(file);
    };

    const startRovers = async () => {
        const response = await fetch('/api/plateau/startall', {
            headers: { "Content-Type": "application/json" },
            credentials: "include",
        });
        const content = await response.json();    // returns bidimensional array; each array within the array contains the visited positions of a rover

        console.log(content); 

        let newBoard = Array(6).fill(0).map(() => new Array(6).fill(""));

        content.forEach((rovers, roverIndex) => {
            rovers.forEach((actions, actionIndex) => {
                actionIndex === 0
                    ? newBoard[actions.y][actions.x] = "\nR" + roverIndex + "_Start"
                    : actionIndex === rovers.length - 1
                        ? newBoard[actions.y][actions.x] += "\nEnd" + roverIndex
                        : newBoard[actions.y][actions.x] += "\nR" + roverIndex + "_Move" + actionIndex;
            })
        })

        setBoard(newBoard);
        console.log(newBoard);
    }

    return (
        <div>
            <label htmlFor="csvInput" style={{ display: "block" }}>
                Enter CSV File
            </label>
            <input
                onChange={handleFileChange}
                id="csvInput"
                name="file"
                type="File"
            />
            <div>
                <button onClick={handleParse}>Parse</button>
            </div>
            <div>
                <button onClick={startRovers}>Start Rovers</button>
            </div>
            <table>
                <tbody>
                    {board.map((rows, rowIndex) => {
                        return (
                            <tr key={rowIndex}>
                                {rows.map((cell, cellIndex) => {
                                    return <td key={`${rowIndex}-${cellIndex}`}> <div className="cell">{cell}</div></td>;
                                })} 
                            </tr>
                        );
                    }).reverse()}
                </tbody>
            </table>
        </div>
        
    );
};

export default Home;