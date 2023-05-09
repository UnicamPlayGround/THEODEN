from django.http import HttpResponseNotFound, FileResponse
from rest_framework import status
from rest_framework.authentication import TokenAuthentication
from rest_framework.decorators import api_view, authentication_classes, permission_classes
from rest_framework.permissions import IsAuthenticated
from rest_framework.response import Response

from Tencu_Framework_Server.controllers import asset_bundles


@api_view(['GET'])
def get_asset_bundle(request):
    """
    Require the query string
    eg:
        /asset/redirect?name=<asset_bundle_name>&os=<destination_os>
    redirects to the specified asset bundle.
    """
    name = request.GET.get('name')
    os = request.GET.get('os')
    file = asset_bundles.get_asset_bundle_file_name(name, os.lower())
    if file is None:
        return HttpResponseNotFound({'error': 'Asset bundle not found.'})
    response = FileResponse(file, filename=name)
    return response


@api_view(['GET'])
def get_asset_bundle_version(request):
    """
    Require the query string
    eg:
        /asset/version?name=<asset_bundle_name>&os=<destination_os>
    Returns the asset bundle version.
    """
    name = request.GET.get('name')
    os = request.GET.get('os')
    version = asset_bundles.get_asset_bundle_version(name, os.lower())
    return Response(version, status=status.HTTP_200_OK)


@api_view(['GET'])
def get_asset_bundle_list(request):
    """
        Require the query string
        eg:
            /asset/list?os=<destination_os>
        Returns the asset bundle list.
    """
    os = request.GET.get('os')
    return Response(asset_bundles.get_asset_bundle_list(os.lower()), status=status.HTTP_200_OK)


@api_view(['GET'])
def get_asset_bundle_dependency_list(request):
    """
        Require the query string
        eg:
            /asset/dependency/list?os=<destination_os>
        Returns the asset bundle dependency.
    """
    os = request.GET.get('os')
    result = asset_bundles.get_asset_bundle_dependency_list(os.lower())
    return Response(result, status=status.HTTP_200_OK)


@api_view(['GET'])
def get_asset_bundle_dependency_version(request):
    """
        Require the query string
        eg:
            /asset/dependency/version?os=<destination_os>
        Returns the asset bundle dependency list version.
    """
    os = request.GET.get('os')
    result = asset_bundles.get_asset_bundle_dependency_version(os.lower())
    return Response(result, status=status.HTTP_200_OK)


@api_view(['POST'])
@authentication_classes([TokenAuthentication])
@permission_classes([IsAuthenticated])
def add_or_edit_asset_bundle(request):
    """
    Require post form with name, file, crc and dependencies (dependencies needed in form of csv, separated by ';').
    Adds the asset bundle.
    """
    name = request.POST.get('name')
    crc = request.POST.get('crc')
    os = request.POST.get('destination_os')
    dependencies = request.POST.get('dependencies')
    codex_stop = request.POST.get('codex_stop')
    if dependencies is not None:
        dependencies = dependencies.split(';')
    else:
        dependencies = []
    if 'assetBundle' not in request.FILES or not request.FILES['assetBundle']:
        return Response({'error': 'No file provided.'}, status=status.HTTP_400_BAD_REQUEST)
    asset_bundle = request.FILES['assetBundle']
    asset_bundle.name = name
    result = asset_bundles.add_or_edit_asset_bundle(name, asset_bundle, crc, dependencies, os.lower(), codex_stop)
    return Response(result, status=status.HTTP_200_OK)


@api_view(['DELETE'])
@authentication_classes([TokenAuthentication])
@permission_classes([IsAuthenticated])
def delete_asset_bundle(request):
    """
    Deletes the asset bundle.
    """
    name = request.GET.get('name')
    os = request.GET.get('os')
    result = asset_bundles.delete_asset_bundle(name, os.lower())
    return Response(result, status=status.HTTP_200_OK)
