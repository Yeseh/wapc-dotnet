namespace Wapc;

public static class WapcConstants
{
    public const string HOST_NAMESPACE = "wapc";
    public const string HOST_CONSOLE_LOG = "__console_log";

    public const string HOST_CALL = "__host_call";
    public const string HOST_RESPONSE = "__host_response";
    public const string HOST_RESPONSE_LEN = "__host_response_len";
    public const string HOST_ERROR = "__host_error";
    public const string HOST_ERROR_LEN = "__host_error_len";

    public const string GUEST_CALL = "__guest_call";
    public const string GUEST_RESPONSE = "__guest_response";
    public const string GUEST_RESPONSE_LEN = "__guest_response_len";
    public const string GUEST_REQUEST = "__guest_request";
    public const string GUEST_ERROR = "__guest_error";

    public const string WAPC_INIT = "wapc_init";
    public const string START = "_start";
    
    public static readonly string[] REQUIRED_STARTS = { START, WAPC_INIT };
}
