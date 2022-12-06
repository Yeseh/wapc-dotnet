#include <mono-wasi/driver.h>
#include <assert.h>
#include <string.h>

MonoMethod* method_GuestCall;

__attribute__((import_name("__host_call")))
int wapc_host_call(
    void* bd_ptr, int bd_len, 
    void* ns_ptr, int ns_len, 
    void* cmd_ptr, int cmd_len, 
    void* payload_ptr, int payload_len);

__attribute__((import_name("__guest_request")))
void wapc_guest_request(void* op_ptr, void* ptr);

__attribute__((import_name("__host_response")))
void wapc_host_response(void* ptr);

__attribute__((import_name("__host_response_len")))
int wapc_host_response_len();

__attribute__((import_name("__guest_response")))
void wapc_guest_response(void* ptr, int len);

__attribute__((import_name("__guest_error")))
void wapc_guest_error(void* ptr, int len);

__attribute__((import_name("__host_error")))
void wapc_host_error(void* ptr);

__attribute__((import_name("__host_host_error_len")))
int wapc_host_error_len();


__attribute__((export_name("__guest_call")))
int wapc_guest_call(int op_len, int msg_len) {

}

void fake_settimeout(int timeout) {
    // Skipping
}

void http_server_attach_internal_calls() {
    printf("Attaching internal calls...")

    // WAPC Host Imports
    mono_add_internal_call ("Wapc.Guest.Wapc::HostCall", wapc_host_call);
    mono_add_internal_call ("Wapc.Guest.Wapc::GuestRequest", wapc_guest_request);
    mono_add_internal_call ("Wapc.Guest.Wapc::HostResponse", wapc_host_response);
    mono_add_internal_call ("Wapc.Guest.Wapc::HostResponseLen", wapc_host_response_len);
    mono_add_internal_call ("Wapc.Guest.Wapc::GuestResponse", wapc_guest_response);
    mono_add_internal_call ("Wapc.Guest.Wapc::GuestError", wapc_guest_error); 
    mono_add_internal_call ("Wapc.Guest.Wapc::HostError", wapc_host_error);
    mono_add_internal_call ("Wapc.Guest.Wapc::HostErrorLen", wapc_host_error_len);

    mono_add_internal_call ("System.Threading.TimerQueue::SetTimeout", fake_settimeout);
}
