from flask import Flask
app = Flask(__name__)
import time 
import requests
#import cv2
import operator
import numpy as np
import operator
#from __future__ import print_function

# Display images within Jupyter

_url = 'https://api.projectoxford.ai/emotion/v1.0/recognize'
_key = 'a5b5547007d54be7aa5bb75555376661'
_maxNumRetries = 10

##############################################################


def processRequest( json, data, headers, params ):

    """
    Helper function to process the request to Project Oxford

    Parameters:
    json: Used when processing images from its URL. See API Documentation
    data: Used when processing image read from disk. See API Documentation
    headers: Used to pass the key information and the data type request
    """

    retries = 0
    result = None

    while True:

        response = requests.request( 'post', _url, json = json, data = data, headers = headers, params = params )

        if response.status_code == 429: 

            print( "Message: %s" % ( response.json()['error']['message'] ) )

            if retries <= _maxNumRetries: 
                time.sleep(1) 
                retries += 1
                continue
            else: 
                print( 'Error: failed after retrying!' )
                break

        elif response.status_code == 200 or response.status_code == 201:

            if 'content-length' in response.headers and int(response.headers['content-length']) == 0: 
                result = None 
            elif 'content-type' in response.headers and isinstance(response.headers['content-type'], str): 
                if 'application/json' in response.headers['content-type'].lower(): 
                    result = response.json() if response.content else None 
                elif 'image' in response.headers['content-type'].lower(): 
                    result = response.content
        else:
            print( "Error code: %d" % ( response.status_code ) )
            print( "Message: %s" % ( response.json()['error']['message'] ) )

        break
        
    return result
    
        
########################################################

# URL direction to image
urlImage = 'https://raw.githubusercontent.com/Microsoft/ProjectOxford-ClientSDK/master/Face/Windows/Data/detection3.jpg'

headers = dict()
headers['Ocp-Apim-Subscription-Key'] = _key
headers['Content-Type'] = 'application/json' 

json = { 'url': urlImage } 
data = None
params = None

@app.route("/")
def hello():
	result = processRequest( json, data, headers, params )
	answer = max(result[0][u'scores'].iteritems(), key=operator.itemgetter(1))[0]
	return answer

#if result is not None:
#    print(result)
    # Load the original image, fetched from the URL
#    arr = np.asarray( bytearray( requests.get( urlImage ).content ), dtype=np.uint8 )
#    img = cv2.cvtColor( cv2.imdecode( arr, -1 ), cv2.COLOR_BGR2RGB )
#
#    renderResultOnImage( result, img )
#
#    ig, ax = plt.subplots(figsize=(15, 20))
#    ax.imshow( img )


if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5435)