/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import 'bootstrap/dist/css/bootstrap.min.css';
import Chart1 from '../Chart1';
import Chart2 from '../Chart2';
import { useState } from 'react';
import { Models } from '../../models/models';

function Container() {
    const [filterModel, setFilterModel] = useState<Models.IFilterModel | undefined>();

    const handleStateChange = (event: any) => {
        const value = event.target.value;
        if (filterModel === undefined) {
            setFilterModel({
                borough: String(value),
                deadlyOnly: false
            })
        } else {
            setFilterModel({
                deadlyOnly: filterModel?.deadlyOnly,
                borough: String(value),
            })
        }
    };
    const handleCheckboxChange = () => {
        let currentState = false;
        if (filterModel !== undefined)
            currentState = filterModel.deadlyOnly;
        setFilterModel({
            deadlyOnly: !currentState,
            borough: filterModel?.borough,
            yearFrom: filterModel?.yearFrom
        });
    };

    return (
        <div>
            <div style={{ width: '100%', height: '100%', display: 'inline-flex', padding: '10px', margin: '5px' }}>
                <label style={{ paddingRight: '50px' }}>
                    <span style={{ paddingRight: '5px' }}>
                        Deadly Only
                    </span>


                    <input type="checkbox" value="true" checked={filterModel?.deadlyOnly}
                        onChange={handleCheckboxChange}>
                    </input>
                </label>
                <label style={{ paddingRight: '5px' }}>State Filter:</label>
                <select onChange={handleStateChange} value={filterModel?.borough}>
                    <option value="">
                    </option>
                    <option value="BRONX">
                        BRONX
                    </option>
                    <option value="BROOKLYN">
                        BROOKLYN
                    </option>
                    <option value="QUEENS">
                        QUEENS
                    </option>
                    <option value="MANHATTAN">
                        MANHATTAN
                    </option>
                    <option value="STATEN ISLAND">
                        STATEN ISLAND
                    </option>
                </select>
            </div>
            <div style={{ width: '100%', height: '100%', display: 'inline-flex', padding: '5px', margin: '5px' }}>
                <div style={{ width: '100%', height: '100%', border: 'solid 1px black', padding: '5px', margin: '5px' }}>
                    <Chart1 filter={
                        filterModel
                    }>
                    </Chart1>
                </div>
                <div style={{ width: '100%', height: '100%', border: 'solid 1px black', padding: '5px', margin: '5px', marginRight: '10px' }}>
                    <Chart2 filter={
                        filterModel
                    }>
                    </Chart2>
                </div>
            </div>
        </div>

    );

}

export default Container;