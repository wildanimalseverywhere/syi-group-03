/* eslint-disable @typescript-eslint/no-unused-vars */
import 'bootstrap/dist/css/bootstrap.min.css';
import { ApiClient } from '../../infrastructure/apiClient';

function Panel(props: { data: ApiClient.DataResponseModel | undefined }) {

    console.log(props.data);
    return (
        props === undefined || props.data === undefined ? null :
            props.data.hasError ?
                <div className="pt-2">
                    <div className="alert alert-danger" role="alert">
                        {props.data.errorMessage}
                    </div>
                </div>
                :
                <div className="pt-2">
                    <div className="alert alert-secondary" role="alert">
                        SQL Query: {props.data.renderedSQLQuery}
                    </div>
                </div>
    );

}

export default Panel;