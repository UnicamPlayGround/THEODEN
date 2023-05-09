from Tencu_Framework_Server.controllers import utilities
from Tencu_Framework_Server.models.asset_bundles.AssetBundles import AssetBundles
from Tencu_Framework_Server.models.asset_bundles.Dependency import Dependency
from Tencu_Framework_Server.serializers.asset_bundles.AssetBundles import AssetBundlesSerializer
from Tencu_Framework_Server.serializers.asset_bundles.Dependency import DependencySerializer


def get_asset_bundle_list(os_name: str):
    lst = AssetBundles.objects.filter(destination_os=os_name)
    return utilities.serialize_object_list(lst, AssetBundlesSerializer)


def get_asset_bundle_file_name(name: str, os_name: str):
    return utilities.get_file_from_database(AssetBundles, {'name': name, 'destination_os': os_name})


def get_asset_bundle_version(name: str, os: str) -> dict:
    return utilities.get_version({'name': 'asset_bundle_' + name + '_' + os})


def add_or_edit_dependency(asset, dependency: str, os: str):
    child = AssetBundles.objects.filter(name=dependency)
    if len(child) == 0:
        return None
    else:
        child = child[0]
    dependency_obj = Dependency.objects.filter(parent=asset["id"], child=child.id)
    return utilities.generic_add_or_edit(dependency_obj, DependencySerializer,
                                         {'parent': asset["id"], 'child': child.id}, 'dependency_' + os)


def add_or_edit_asset_bundle(name: str, asset_bundle, crc: str, dependencies: list, os: str, codex_stop: str) -> dict:
    if codex_stop == 'null':
        codex_stop = None
    asset = AssetBundles.objects.filter(name=name, destination_os=os)
    if len(asset) > 0:
        asset[0].file.delete()
    asset_bundle_data = utilities.generic_add_or_edit(asset, AssetBundlesSerializer,
                                                      {'name': name, 'file': asset_bundle, 'crc': crc,
                                                       'destination_os': os,
                                                       'codex_stop': codex_stop},
                                                      'asset_bundle_' + name + '_' + os)
    for dependency in dependencies:
        add_or_edit_dependency(asset_bundle_data, dependency, os)
    return asset_bundle_data


def delete_asset_bundle(name: str, os: str) -> dict:
    return utilities.generic_delete(AssetBundles,
                                    {'name': name, 'destination_os': os},
                                    'asset_bundle_' + name + '_' + os)


def get_asset_bundle_dependency_list(os: str):
    dependency_obj = Dependency.objects.all()
    out_list = []
    for dependency in dependency_obj:
        # asserting that dependencies between asset bundles are related to the same os, then
        # dependency.parent.destination_os == dependency.child.destination_os
        if dependency.parent.destination_os == os:
            out_list.append({"parent": dependency.parent.name, "parent_id": dependency.parent.codex_stop.id,
                             "child": dependency.child.name, "child_id": dependency.child.id})
    return out_list


def get_asset_bundle_dependency_version(os: str):
    return utilities.get_version({'name': 'dependency_' + os})
