function insertframe_adjustHeight(e,t){var r=jQuery("#"+e.name),n=(r.get(0).contentDocument?r.get(0).contentDocument:r.get(0).contentWindow.document).body.parentElement,i=n.scrollHeight<n.offsetHeight?n.scrollHeight:n.offsetHeight;if(jQuery.browser.msie)i=n.scrollHeight;r.height(i+t)}function inserframe_crossdomain_adjustHeight(e,t){var r=jQuery("#"+e.name);jQuery.postMessage({event:"insertframe_get_height",url:location.href,name:e.name,offset:t},r.attr("src"),r.get(0).contentWindow)}function insertframe_get_params(e){for(var t,r=/\+/g,n=/([^&=]+)=?([^&]*)/g,i=function(e){return decodeURIComponent(e.replace(r," "))},a={};t=n.exec(e);)a[i(t[1])]=i(t[2]);return a}function insertframe_attach_child_receive_event(e){jQuery.receiveMessage(function(e){insertframe_receive_event(e)},function(t){var r=new RegExp(e.join("|"),"i");return null!=t.match(r)})}function insertframe_attach_parent_receive_event(){jQuery.receiveMessage(function(e){var t=insertframe_get_params(e.data),r=jQuery("#"+t.name),n=document.createElement("a");n.href=r.attr("src"),"insertframe_return_height"==t.event&&e.origin==n.protocol+"//"+n.hostname&&r.height(1*t.height+1*t.offset)},function(e){return!0})}function insertframe_receive_event(e){var t=insertframe_get_params(e.data);if("insertframe_get_height"==t.event){var r=$("body").height();t.event="insertframe_return_height",t.height=r,$.postMessage(t,t.url)}}